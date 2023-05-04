from mesa import Model, Agent
from mesa.space import ContinuousSpace
from mesa.time import RandomActivation
from mesa.visualization.modules import CanvasGrid
from mesa.visualization.ModularVisualization import ModularServer
from mesa.visualization.UserParam import UserSettableParameter
import random


class MyAgent(Agent):
    def __init__(self, unique_id, model):
        super().__init__(unique_id, model)
        self.x = random.uniform(0, self.model.grid.width)
        self.y = random.uniform(0, self.model.grid.height)

    def step(self):
        self.x += random.uniform(-1, 1)
        self.y += random.uniform(-1, 1)
        self.x = self.model.space.x_min + ((self.x - self.model.space.x_min) % self.model.space.width)
        self.y = self.model.space.y_min + ((self.y - self.model.space.y_min) % self.model.space.height)


class MyModel(Model):
    def __init__(self, N, width, height):
        self.num_agents = N
        self.space = ContinuousSpace(width, height, True)
        self.schedule = RandomActivation(self)
        self.grid = CanvasGrid(lambda x: "Agent", width, height, 500, 500)
        
        for i in range(self.num_agents):
            a = MyAgent(i, self)
            self.schedule.add(a)
            self.space.place_agent(a, (a.x, a.y))

    def step(self):
        self.schedule.step()

    def run_model(self, n):
        for i in range(n):
            self.step()
            self.grid_portrayal = self.grid.get_portrayal_object()
            self.datacollector.collect(self)


model_params = {
    "N": UserSettableParameter(
        "slider",
        "Number of agents",
        10,
        1,
        50,
        1
    ),
    "width": UserSettableParameter(
        "slider",
        "Width",
        50,
        10,
        100,
        1
    ),
    "height": UserSettableParameter(
        "slider",
        "Height",
        50,
        10,
        100,
        1
    ),
}

server = ModularServer(
    MyModel,
    [CanvasGrid],
    "My Model",
    model_params,
)
server.port = 8521
server.launch()