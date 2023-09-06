#!/usr/bin/env python

import rospy
from std_msgs.msg import Int64
from sensor_msgs.msg import Image
from geometry_msgs.msg import PoseStamped, Wrench
import time

class LatencyTestNode:
    def __init__(self):
        rospy.init_node('latency_test_node', anonymous=True)
        self.received_timestamp = None
        # self.latency_sub = rospy.Subscriber('/LatencyTest', Int64, self.latency_callback)
        # self.latency_sub = rospy.Subscriber('/camera/image_raw', Image, self.latency_callback)
        # self.latency_sub = rospy.Subscriber('/orb_slam3/camera_pose', PoseStamped, self.latency_callback)
        self.latency_sub = rospy.Subscriber('/bluerov2/thruster_manager/input', Wrench, self.latency_callback)

    def latency_callback(self, msg):
        # self.received_timestamp = msg.data
        # self.calculate_delay()
        if msg is not None:
            current_timestamp = int(time.time() * 1000)
            print("Current time image published")
            print(current_timestamp)

    def calculate_delay(self):
        if self.received_timestamp is not None:
            current_timestamp = int(time.time() * 1000)
            delay = current_timestamp - self.received_timestamp
            rospy.loginfo("Received Timestamp: {}, Current Timestamp: {}, Delay: {} milliseconds".format(self.received_timestamp, current_timestamp, delay))
        # return

if __name__ == '__main__':
    try:
        latency_test_node = LatencyTestNode()
        rospy.spin()  # Keep the node running
    except rospy.ROSInterruptException:
        pass
