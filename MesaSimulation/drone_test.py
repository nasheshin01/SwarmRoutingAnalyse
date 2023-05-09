import unittest
from unittest.mock import Mock
from fanet_model import FANET
from data_agent import DataAgent
from drone import Drone
import json

class TestDrone(unittest.TestCase):

    def setUp(self) -> None:
        with open("settings.json", 'r') as file:
            config = json.load(file)
        self.model = FANET(config)
        self.model.const_loading_speed = 1
        self.drone1 = Drone(0, self.model, 1, 1, False, False)
        self.drone2 = Drone(1, self.model, 2, 2, False, False)
        self.dataAgent = DataAgent(0, self.model, 1, 1, self.drone1, 1)
        self.model.grid.place_agent(self.drone1, (1, 1))
        self.model.grid.place_agent(self.drone2, (2, 2))
        self.model.grid.place_agent(self.dataAgent, (1, 1))


    def test_enable_hibernation(self):
        self.drone1.enable_hibernation()
        self.assertEqual(self.drone1.hibernation_status, "Hibernate")

    def test_disable_hibernation(self):
        self.drone1.disable_hibernation()
        self.assertEqual(self.drone1.hibernation_status, "NoHibernate")

    def test_random_move(self):
        self.drone1.random_move((10, 10))
        self.assertGreater(self.drone1.x, -1)
        self.assertGreater(self.drone1.y, -1)
        self.assertLess(self.drone1.x, self.drone1.x + 1)
        self.assertLess(self.drone1.y, self.drone1.y + 1)

    def test_add_best_path(self):
        best_path = []
        self.drone1.add_best_path(best_path, 1)

        self.assertEqual(self.drone1.best_path, best_path)
        self.assertEqual(self.drone1.best_path_step_count, 1)
        self.assertEqual(self.drone1.best_path_id, 1)

        self.drone1.add_best_path([1, 2], 2)
        self.assertEqual(self.drone1.best_path, best_path)
        self.assertEqual(self.drone1.best_path_step_count, 1)
        self.assertEqual(self.drone1.best_path_id, 1)

        best_path = [1, 2]
        self.drone1.add_best_path([1, 2], 0)
        self.assertEqual(self.drone1.best_path, best_path)
        self.assertEqual(self.drone1.best_path_step_count, 0)
        self.assertEqual(self.drone1.best_path_id, 2)

    def test_update_best_path(self):
        best_path = []
        self.drone1.add_best_path(best_path, 1)

        self.drone1.update_best_path(5, 2)
        self.assertNotEqual(self.drone1.best_path_step_count, 5)

        self.drone1.update_best_path(5, 1)
        self.assertEqual(self.drone1.best_path_step_count, 5)

    def create_worker(self):
        self.drone1.create_worker()

        self.assertEqual(self.drone1.worker_id, 312315)

if __name__ == '__main__':
    unittest.main()
    