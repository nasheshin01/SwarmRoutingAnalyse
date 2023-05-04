from mesa import Agent, Model
from utils import euclidean_distance


class MovingAgent(Agent):

    def __init__(self, unique_id: int, model: Model, x: int, y: int) -> None:
        super().__init__(unique_id, model)

        self.x = x
        self.y = y

    def move(self, new_x: int, new_y: int):
        self.x = new_x
        self.y = new_y

        self.model.grid.move_agent(self, (new_x, new_y))

    def get_distance(self, agent):
        x1, y1 = self.x, self.y
        x2, y2 = agent.x, agent.y
        return euclidean_distance(x1, y1, x2, y2)