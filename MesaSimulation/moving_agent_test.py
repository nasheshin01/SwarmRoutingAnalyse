import unittest
from unittest.mock import Mock
from fanet_model import FANET
from moving_agent import MovingAgent
import json

class TestMovingAgent(unittest.TestCase):

    def setUp(self) -> None:
        with open("settings.json", 'r') as file:
            config = json.load(file)
        self.model = FANET(config)
        self.agent = MovingAgent(0, self.model, 0, 0)
        self.agent2 = MovingAgent(1, self.model, 3, 4)
        self.model.grid.place_agent(self.agent, (0, 0))
        self.model.grid.place_agent(self.agent2, (3, 4))


    def test_move(self):
        self.agent.move(1, 2)
        self.assertEqual(self.agent.x, 1)
        self.assertEqual(self.agent.y, 2)

    def test_get_distance(self):
        dist = self.agent.get_distance(self.agent2)
        self.assertEqual(dist, 5)

if __name__ == '__main__':
    unittest.main()
    