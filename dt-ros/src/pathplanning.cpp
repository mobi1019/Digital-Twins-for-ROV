// This code is gotten from https://github.com/ayushgaud/path_planning/blob/master/src/old_path_planning.cpp
#include <ros/ros.h>
#include <ompl/geometric/SimpleSetup.h>
#include <ompl/base/spaces/SE3StateSpace.h>
#include <ompl/base/Planner.h>
#include <ompl/base/goals/GoalState.h>
#include <octomap/octomap.h>

#include <octomap_msgs/Octomap.h>
#include <octomap_msgs/conversions.h>
#include <octomap_ros/conversions.h>
#include <ompl/geometric/planners/rrt/RRTstar.h>
#include <ompl/geometric/planners/rrt/RRTConnect.h>
#include <ompl/geometric/planners/rrt/InformedRRTstar.h>
#include <ompl/geometric/SimpleSetup.h>
#include <ompl/config.h>
#include <fstream>
#include <nav_msgs/Path.h>
#include <geometry_msgs/Pose.h>
#include <geometry_msgs/PoseStamped.h>
#include <Eigen/Dense>

#include "fcl/config.h"
#include "fcl/octree.h"
#include "fcl/traversal/traversal_node_octree.h"
#include "fcl/collision.h"
#include "fcl/broadphase/broadphase.h"
#include "fcl/math/transform.h"

#include "dt_ros/pp.h"


namespace ob = ompl::base;
namespace og = ompl::geometric;
octomap::OcTree octree(0.2);
ros::Publisher path_pub;
geometry_msgs::Point localizer;
octomap::point3d minBound, maxBound;

std::shared_ptr<fcl::CollisionGeometry> rov(new fcl::Box(0.457, 0.338, 0.254)); // bluerov geometry
fcl::OcTree* tree = new fcl::OcTree(std::shared_ptr<const octomap::OcTree>(new octomap::OcTree(0.2)));
fcl::CollisionObject treeObj((std::shared_ptr<fcl::CollisionGeometry>(tree)));
fcl::CollisionObject uuvObject(rov);

void pointCloudCallback(const geometry_msgs::PoseStamped::ConstPtr& msg) {
    
    geometry_msgs::Point localizer = msg->pose.position;
    std::cout<<localizer.x;
}

void loadOctomap(const std::string &filename, octomap::OcTree &octree) {
    octree.readBinary(filename);
    for (octomap::OcTree::leaf_iterator it = octree.begin_leafs(); it != octree.end_leafs(); ++it) {
        octomap::point3d nodeCoord = it.getCoordinate();
        if (it->getValue() > octree.getOccupancyThres()) {
            if (nodeCoord.x() < minBound.x()) minBound.x() = nodeCoord.x();
            if (nodeCoord.y() < minBound.y()) minBound.y() = nodeCoord.y();
            if (nodeCoord.z() < minBound.z()) minBound.z() = nodeCoord.z();

            if (nodeCoord.x() > maxBound.x()) maxBound.x() = nodeCoord.x();
            if (nodeCoord.y() > maxBound.y()) maxBound.y() = nodeCoord.y();
            if (nodeCoord.z() > maxBound.z()) maxBound.z() = nodeCoord.z();
            }
        }

    // Print the bounds
    std::cout << "Printing out bounds** ";
    std::cout << "Min Bound: " << minBound << std::endl;
    std::cout << "Max Bound: " << maxBound << std::endl;


    fcl::OcTree* tree = new fcl::OcTree(std::shared_ptr<const octomap::OcTree>(&octree));
    fcl::CollisionObject temp((std::shared_ptr<fcl::CollisionGeometry>(tree)));
    treeObj = temp;
}

bool isStateValid(const ob::State *state) {
    // cast the abstract state type to the type we expect
	const ob::SE3StateSpace::StateType *se3state = state->as<ob::SE3StateSpace::StateType>();

    // extract the first component of the state and cast it to what we expect
	const ob::RealVectorStateSpace::StateType *pos = se3state->as<ob::RealVectorStateSpace::StateType>(0);

    // extract the second component of the state and cast it to what we expect
	const ob::SO3StateSpace::StateType *rot = se3state->as<ob::SO3StateSpace::StateType>(1);

    // check validity of state Fdefined by pos & rot
	fcl::Vec3f translation(pos->values[0],pos->values[1],pos->values[2]);
	fcl::Quaternion3f rotation(rot->w, rot->x, rot->y, rot->z);
	uuvObject.setTransform(rotation, translation);
	fcl::CollisionRequest requestType(1,false,1,false);
	fcl::CollisionResult collisionResult;
	fcl::collide(&uuvObject, &treeObj, requestType, collisionResult);

	return(!collisionResult.isCollision());
}

bool run (dt_ros::pp::Request &req, dt_ros::pp::Response &res){
     // construct the state space we are planning in
	ob::StateSpacePtr space;

	// construct an instance of  space information from this state space
	ob::SpaceInformationPtr si;

	// create a problem instance
	ob::ProblemDefinitionPtr pdef;

    // Load the octomap from the file
    std::string octomapFilename = "/home/favour/catkin_ws/octomap.bt"; 
    loadOctomap(octomapFilename, octree);

    // Create an OMPL state space for SE3 planning (3D space with position and orientation)
    space = ob::StateSpacePtr(new ob::SE3StateSpace());

    // Set bounds for the state space 
    ob::RealVectorBounds bounds(3);

    bounds.setLow(0,minBound(0));
    bounds.setHigh(0,maxBound(0));
    bounds.setLow(1,minBound(1));
    bounds.setHigh(1,maxBound(1));
    bounds.setLow(2,minBound(2)-1.0);
    bounds.setHigh(2,maxBound(2));

    space->as<ob::SE3StateSpace>()->setBounds(bounds);

    // Create a space information for the state space
    si = ob::SpaceInformationPtr(new ob::SpaceInformation(space));

    
    // Set the state validity checker (collision checking)
    si->setStateValidityChecker(std::bind(&isStateValid, std::placeholders::_1));

    ob::ScopedState<ob::SE3StateSpace> start(space);
    start->setXYZ(localizer.x, localizer.y, localizer.z); // Set desired start position
    start->as<ob::SO3StateSpace::StateType>(1)->setIdentity(); // Set identity rotation

    ob::ScopedState<ob::SE3StateSpace> goal(space);
    goal->setXYZ(req.x, req.y, req.z); // Set  desired goal position
    goal->as<ob::SO3StateSpace::StateType>(1)->setIdentity(); // Set identity rotation

    // Create a problem definition
    pdef = ob::ProblemDefinitionPtr(new ob::ProblemDefinition(si));

    pdef->setStartAndGoalStates(start, goal);

    // Create a planner
    ob::PlannerPtr plan(new og::RRTstar(si));


    // Set the planner to the problem definition
    plan->setProblemDefinition(pdef);
    plan->setup();
        
    // print the settings for this space
	si->printSettings(std::cout);

    // print the problem settings
	pdef->print(std::cout);
    // Solve the problem
    
    ob::PlannerStatus solved = plan->solve(2.0);

    if (solved) {
        std::cout<<"just before solved";
        // Get the path from the problem definition
        ob::PathPtr pth = pdef->getSolutionPath();
        og::PathGeometric* path = pdef->getSolutionPath()->as<og::PathGeometric>();

        // Print the path to the console
        path->print(std::cout);
        std::cout<<"published paths results";
        
        nav_msgs::Path path_msg;
        path_msg.header.frame_id = "map"; 
        for (std::size_t i = 0; i < path->getStateCount(); ++i) {
            const auto *state = path->getState(i)->as<ob::SE3StateSpace::StateType>();
            geometry_msgs::PoseStamped pose;
            pose.header.frame_id = "map"; 
            pose.pose.position.x = state->getX();
            pose.pose.position.y = state->getY();
            pose.pose.position.z = state->getZ();
            pose.pose.orientation.x = state->rotation().x;
            pose.pose.orientation.y = state->rotation().y;
            pose.pose.orientation.z = state->rotation().z;
            pose.pose.orientation.w = state->rotation().w;
            path_msg.poses.push_back(pose);
        }
        ros::NodeHandle nh;
        ros::Publisher path_pub = nh.advertise<nav_msgs::Path>("/planned_path", 10);
        ros::Duration(1).sleep();
        path_pub.publish(path_msg);
    } else {
        ROS_ERROR("Failed to find a solution!");
        return false;
    }

    return true;
}

int main(int argc, char **argv) {
    ros::init(argc, argv, "pathplanning");
    ros::NodeHandle nh;
    ros::Subscriber point_cloud_sub = nh.subscribe("/orb_slam3/camera_pose", 1, pointCloudCallback);


    ros::ServiceServer service = nh.advertiseService("pp", run);
    ROS_INFO("Ready to plan a path");
    ros::spin();

    return 0;
}
