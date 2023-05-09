import unittest
from unittest.mock import Mock
from mesa import Model
from worker import Worker
from package_generator import Package
from drone import Drone
import json
from fanet_model import FANET

class TestWorker(unittest.TestCase):

    def setUp(self):
        with open("settings.json", 'r') as file:
            config = json.load(file)
        self.model = FANET(config)
        self.model.current_step = 1
        self.current_drone = Drone(1, self.model, 0, 0, True, False)
        self.package = Package(1, 1)
        self.worker = Worker(1, self.model, self.current_drone, self.package)

    def test_worker_init(self):
        self.assertEqual(self.worker.unique_id, 1)
        self.assertEqual(self.worker.model, self.model)
        self.assertEqual(self.worker.current_drone, self.current_drone)
        self.assertEqual(self.worker.worker_state, "Sending")
        self.assertEqual(self.worker.source_drone, self.current_drone)
        self.assertEqual(self.worker.best_path, self.current_drone.best_path)
        self.assertEqual(self.worker.best_path_id, self.current_drone.best_path_id)
        self.assertEqual(self.worker.step_counts_to_go_back, 0)
        self.assertEqual(self.worker.package, self.package)
        self.assertIsNone(self.worker.next_drone)
        self.assertFalse(self.worker.is_next_drone_chosen)
        self.assertFalse(self.worker.is_next_drone_busy)
        self.assertFalse(self.worker.is_next_drone_out_of_radius)
        self.assertFalse(self.worker.is_current_drone_source)
        self.assertFalse(self.worker.is_current_drone_destination)

    def test_change_state_to_going_to_start(self):
        self.worker.change_state_to_going_to_start()
        self.assertEqual(self.worker.worker_state, "GoingToStart")
        self.assertEqual(self.worker.size, self.model.scout_size)

    def test_change_state_to_send(self):
        self.worker.change_state_to_send()
        self.assertEqual(self.worker.worker_state, "Sending")

    def test_change_state_to_send_end(self):
        self.worker.change_state_to_send_end()
        self.assertEqual(self.worker.worker_state, "SendingEnded")

    def test_update_best_path(self):
        self.worker.update_best_path()
        self.assertEqual(self.current_drone.best_path_id, self.worker.best_path_id)
        self.assertEqual(self.current_drone.best_path_step_count, self.worker.step_counts_to_go_back)

    def test_load_to_next_drone(self):
        next_drone = Drone(2, self.model, 1, 1, False, False)
        self.worker.start_loading = Mock()
        self.worker.next_drone = next_drone
        self.worker.load_to_next_drone()
        self.worker.start_loading.assert_called_once_with(next_drone)

    def test_choose_next_drone_forward(self):
        next_drone = Drone(2, self.model, 1, 1, False, False)
        self.worker.current_drone = self.current_drone
        self.worker.best_path = [self.current_drone, next_drone]
        self.worker.choose_next_drone_forward()
        self.assertEqual(self.worker.next_drone, next_drone)
        self.assertTrue(self.worker.is_next_drone_chosen)
        self.assertFalse(self.worker.is_next_drone_busy)
        self.assertFalse(self.worker.is_next_drone_out_of_radius)

    def test_choose_next_drone_back(self):
        previous_drone = Drone(2, self.model, 1, 1, False, False)
        self.worker.current_drone = self.current_drone
        self.worker.best_path = [previous_drone, self.current_drone]
        self.worker.choose_next_drone_back()
        self.assertEqual(self.worker.next_drone, previous_drone)
        self.assertTrue(self.worker.is_next_drone_chosen)
        self.assertFalse(self.worker.is_next_drone_busy)
        self.assertFalse(self.worker.is_next_drone_out_of_radius)

if __name__ == '__main__':
    unittest.main()