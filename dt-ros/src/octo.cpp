#include <ros/ros.h>
#include <sensor_msgs/PointCloud2.h>
#include <geometry_msgs/Pose.h>
#include <octomap/octomap.h>
#include <octomap_msgs/conversions.h>
#include <octomap_msgs/Octomap.h>
#include <octomap_msgs/GetOctomap.h>
#include <pcl_conversions/pcl_conversions.h>
#include <pcl/point_cloud.h>
#include <pcl/point_types.h>
#include "dt_ros/pp.h"

using namespace octomap;
bool callService = false;
dt_ros::pp srv;
ros::ServiceClient client;
OcTree tree(0.2); // Set the resolution of the octomap here

void pointCloudCallback(const sensor_msgs::PointCloud2::ConstPtr& msg) {
    // point cloud callback for creating octomap
    pcl::PCLPointCloud2 pcl_pc2;
    pcl_conversions::toPCL(*msg, pcl_pc2);
    pcl::PointCloud<pcl::PointXYZ> pcl_cloud;
    pcl::fromPCLPointCloud2(pcl_pc2, pcl_cloud);

    for (pcl::PointCloud<pcl::PointXYZ>::const_iterator it = pcl_cloud.begin(); it != pcl_cloud.end(); ++it) {
        if (!std::isnan(it->x) && !std::isnan(it->y) && !std::isnan(it->z)) {
            tree.updateNode(point3d(it->x, it->y, it->z), true);
        }
    }

    tree.updateInnerOccupancy();
}


void plannerCallback(const geometry_msgs::Pose::ConstPtr& msg) {
    // if message recieved subscriber call back saves octomap then sends pose to path planning node
    // save octomap
    if (tree.writeBinary("octomap.bt")) {
        ROS_INFO("Octomap saved to octomap.bt");
    } else {
        ROS_ERROR("Failed to save the octomap!");
    }
    ros::Duration(1).sleep();

    // Send pose goal
    geometry_msgs::Point position = msg->position;
    geometry_msgs::Quaternion orientation = msg->orientation;
    // dt_ros::pp srv;
    srv.request.x = position.x;
    srv.request.y = position.y;
    srv.request.z = position.z;
    // callService
    ros::NodeHandle nh;
    ros::ServiceClient client = nh.serviceClient<dt_ros::pp>("pp");
    if (client.call(srv)){
        ROS_INFO("service called");
    } else {
        ROS_INFO("unable to call service");
    }
}

int main(int argc, char** argv) {
    ros::init(argc, argv, "octomap_builder");
    ros::NodeHandle nh;
    ros::Subscriber point_cloud_sub = nh.subscribe("/orb_slam3/all_points", 1, pointCloudCallback);
    ros::Subscriber path_plan_sub = nh.subscribe("/path_planner", 1, plannerCallback);
    
    ros::Rate rate(10); // Set the rate according to your needs
    ros::spin();
    return 0;
}
