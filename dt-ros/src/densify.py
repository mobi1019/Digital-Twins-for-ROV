#!/usr/bin/env python
import rospy
import numpy as np
import sensor_msgs.point_cloud2 as pc2
from sensor_msgs.msg import PointCloud2
from scipy.interpolate import griddata
import time

def interpolate_point_cloud(src_points, target_shape):
    src_points = np.array(src_points)
    src_x = src_points[:, 0]
    src_y = src_points[:, 1]
    src_z = src_points[:, 2]

    target_x = np.linspace(min(src_x), max(src_x), target_shape[0])
    target_y = np.linspace(min(src_y), max(src_y), target_shape[1])
    target_xx, target_yy = np.meshgrid(target_x, target_y)

    target_points = np.vstack((target_xx.ravel(), target_yy.ravel())).T

    target_z = griddata((src_x, src_y), src_z, (target_xx, target_yy), method='linear')

    interpolated_points = np.column_stack((target_points, target_z.ravel()))
    print("interpolated points")
    return interpolated_points

def point_cloud_callback(msg):
    global src_points
    if len(msg.data) == 0:
        print("empty data")
        return
    src_points = []
    for p in pc2.read_points(msg,skip_nans=True):
        src_points.append(p)

    
  

if __name__ == '__main__':
    rospy.init_node('point_cloud_interpolation')
    rospy.Subscriber('/orb_slam3/all_points', PointCloud2, point_cloud_callback)

    interpolated_pub = rospy.Publisher('/dense_cloud', PointCloud2, queue_size=10)

    target_shape = (100, 100)  # Set the target shape for the interpolated point cloud

    rate = rospy.Rate(10)  # 10 Hz

    while not rospy.is_shutdown():
        if 'src_points' in globals():
            interpolated_points = interpolate_point_cloud(src_points, target_shape)
            interpolated_cloud_msg = pc2.create_cloud_xyz32(PointCloud2().header, interpolated_points)
            interpolated_pub.publish(interpolated_cloud_msg)
            print("time in milliseconds densify:" )
            print(int(time.time() * 1000))

        rate.sleep()
