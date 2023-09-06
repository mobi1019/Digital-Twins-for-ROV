cmake_minimum_required(VERSION 3.0.2)
project(dt_ros)

## Compile as C++11, supported in ROS Kinetic and newer
# add_compile_options(-std=c++11)

## Find catkin macros and libraries
## if COMPONENTS list like find_package(catkin REQUIRED COMPONENTS xyz)
## is used, also find other catkin packages
find_package(catkin REQUIRED COMPONENTS
  roscpp
  rospy
  std_msgs
  nav_msgs
  geometry_msgs
  visualization_msgs
  sensor_msgs
  pcl_conversions
  octomap_msgs
  octomap_ros
  pcl_ros
  message_generation
)
find_package(octomap REQUIRED)
find_package(OMPL REQUIRED)
find_package(fcl QUIET)
if (fcl_FOUND)
  # convert cmake target to variables for catkin_package DEPENDS
  if(TARGET ${FCL_LIBRARIES})
    get_target_property(LIBFCL_LOCATION ${FCL_LIBRARIES} LOCATION)
    get_target_property(LIBFCL_INTERFACE_LINK_LIBRARIES ${FCL_LIBRARIES} INTERFACE_LINK_LIBRARIES)
    get_target_property(LIBFCL_INCLUDE_DIRS ${FCL_LIBRARIES} INTERFACE_INCLUDE_DIRECTORIES)
    set(LIBFCL_LIBRARIES ${LIBFCL_LOCATION} ${LIBFCL_INTERFACE_LINK_LIBRARIES})
  else()
    set(LIBFCL_LIBRARIES ${FCL_LIBRARIES})
    set(LIBFCL_INCLUDE_DIRS ${FCL_INCLUDE_DIRS})
  endif()
else()
  find_package(PkgConfig REQUIRED)
  pkg_check_modules(LIBFCL_PC REQUIRED fcl)
  set(LIBFCL_INCLUDE_DIRS ${LIBFCL_PC_INCLUDE_DIRS})
  # find *absolute* paths to LIBFCL_LIBRARIES
  set(LIBFCL_LIBRARIES)
  foreach(_lib ${LIBFCL_PC_LIBRARIES})
    find_library(_lib_${_lib} ${_lib} HINTS ${LIBFCL_PC_LIBRARY_DIRS})
    list(APPEND LIBFCL_LIBRARIES ${_lib_${_lib}})
  endforeach()
endif()

add_service_files(
   FILES
   pp.srv
)

add_message_files(
  FILES
  Path.msg
)

generate_messages(
  DEPENDENCIES
  nav_msgs
  geometry_msgs
  std_msgs

)

catkin_package(
  INCLUDE_DIRS include
  LIBRARIES ${PROJECT_NAME}
  CATKIN_DEPENDS roscpp sensor_msgs  nav_msgs geometry_msgs std_msgs visualization_msgs pcl_conversions pcl_ros octomap_msgs octomap_ros message_runtime
)

include_directories(
  include
  ${catkin_INCLUDE_DIRS}
  ${OCTOMAP_INCLUDE_DIRS}
  ${PCL_INCLUDE_DIRS}
  ${OMPL_INCLUDE_DIRS}
  ${Boost_INCLUDE_DIR}
  ${EIGEN_INCLUDE_DIR}
  ${LIBFCL_INCLUDE_DIRS}
  )

add_executable(octo src/octo.cpp)
target_link_libraries(octo
  ${catkin_LIBRARIES}
  ${OCTOMAP_LIBRARIES}
)
add_executable(pathplanning src/pathplanning.cpp)
target_link_libraries(pathplanning
  ${catkin_LIBRARIES}
  ${OMPL_LIBRARIES}
  fcl
  ${LIBFCL_LIBRARIES}
  ${EIGEN_INCLUDE_DIR}
)
message("ompl llibraries: ${OMPL_LIBRARIES}")