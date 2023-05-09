import unittest
from unittest.mock import Mock
from fanet_model import FANET
from data_agent import DataAgent
from drone import Drone
import json

class TestDataAgent(unittest.TestCase):

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


    def test_start_loading(self):
        self.dataAgent.start_loading(self.drone2)

        self.assertTrue(self.dataAgent.is_loading)
        self.assertEqual(self.dataAgent.agent_load_to, self.drone2)
        self.assertEqual(self.dataAgent.agent_load_to.current_loading_agent, self.dataAgent)

    def test_update_loading_loading_not_finished(self):
        self.dataAgent.start_loading(self.drone2)
        self.dataAgent.update_loading()
        
        self.assertTrue(self.dataAgent.is_loading)

    def test_update_loading_loading_finished(self):
        self.dataAgent.start_loading(self.drone2)
        self.dataAgent.update_loading()
        self.dataAgent.update_loading()
        self.dataAgent.update_loading()
        
        self.assertFalse(self.dataAgent.is_loading)
        self.assertEqual(self.dataAgent.loaded_size, 0)
        self.assertIsNone(self.dataAgent.agent_load_to)
        self.assertEqual(self.dataAgent.current_drone, self.drone2)
        self.assertIsNone(self.drone2.current_loading_agent)
        self.assertEqual(self.dataAgent.x, 2)
        self.assertEqual(self.dataAgent.y, 2)

if __name__ == '__main__':
    unittest.main()
    