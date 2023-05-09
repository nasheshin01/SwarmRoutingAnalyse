import unittest
from fanet_model import FANET
from drone import Drone
from scout import Scout
import json


class TestScout(unittest.TestCase):
    
    def setUp(self):
        with open("settings.json", 'r') as file:
            config = json.load(file)

        self.model = FANET(config)
        self.drone1 = Drone(1, self.model, 0, 0, True, False)
        self.drone2 = Drone(2, self.model, 10, 10, True, False)
        self.scout = Scout(1, self.model, self.drone1, 5)
        self.scout.path.append(self.drone2)
        
    def test_add_best_path(self):
        self.scout.add_best_path()
        self.assertEqual(self.scout.path, self.drone1.best_path)
        
    def test_change_state_to_scout(self):
        self.scout.change_state_to_scout()
        self.assertEqual(self.scout.scout_state, "Scouting")
        
    def test_change_state_to_going_to_start(self):
        self.scout.change_state_to_going_to_start()
        self.assertEqual(self.scout.scout_state, "GoingToStart")
        
    def test_change_state_to_scout_end(self):
        self.scout.change_state_to_scout_end()
        self.assertEqual(self.scout.scout_state, "ScoutingEnded")

if __name__ == '__main__':
    unittest.main()