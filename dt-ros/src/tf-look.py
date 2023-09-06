#!/usr/bin/env python

import rospy
import tf

if __name__ == "__main__":
    rospy.init_node("camera_to_body_transform")

    listener = tf.TransformListener()

    rate = rospy.Rate(10.0)
    while not rospy.is_shutdown():
        try:
            # Look up the transformation from camera frame to body frame
            (trans, rot) = listener.lookupTransform("bluerov2/base_link", "bluerov2/imu_link",rospy.Time(0))
            
            # Print the translation and rotation
            rospy.loginfo("Camera to Body Translation: {}".format(trans))
            rospy.loginfo("Camera to Body Rotation: {}".format(rot))

        except (tf.LookupException, tf.ConnectivityException, tf.ExtrapolationException):
            rospy.logwarn("Transform lookup failed!")
        
        rate.sleep()