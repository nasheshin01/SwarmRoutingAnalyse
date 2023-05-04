from mesa import Agent, Model
from mesa.time import RandomActivation
from mesa.space import MultiGrid
from mesa.visualization.modules import CanvasGrid
from mesa.visualization.ModularVisualization import ModularServer
from mesa.visualization.modules import ChartModule
import random
from fanet_model import FANET

from drone import Drone
from scout import Scout
from worker import Worker
import json


def agent_portrayal(agent):
    """Draw the agents as red points on the grid."""
    if isinstance(agent, Drone):
        if agent.is_source:
            portrayal = {"Shape": "circle", "Color": "green", "Filled": "true", "Layer": 0, "r": 0.5}
        elif agent.is_destination:
            portrayal = {"Shape": "circle", "Color": "red", "Filled": "true", "Layer": 0, "r": 0.5}
        else:
            portrayal = {"Shape": "circle", "Color": "grey", "Filled": "true", "Layer": 0, "r": 0.5}
    elif isinstance(agent, Scout):
        if agent.scout_state == "Scouting":
            portrayal = {"Shape": "circle", "Color": "red", "Filled": "true", "Layer": 1, "r": 0.2}
        else:
            portrayal = {"Shape": "circle", "Color": "yellow", "Filled": "true", "Layer": 1, "r": 0.2}
    elif isinstance(agent, Worker):
        if agent.worker_state == "Sending":
            portrayal = {"Shape": "circle", "Color": "green", "Filled": "true", "Layer": 1, "r": 0.2}
        else:
            portrayal = {"Shape": "circle", "Color": "black", "Filled": "true", "Layer": 1, "r": 0.2}

    portrayal["x"] = agent.x
    portrayal["y"] = agent.y
    return portrayal

def text_display(model: FANET):
    logs = model.logs

    out = ""
    for log in logs[-5:][::-1]:
        out += log + "<br>"
    
    return out

if __name__ == "__main__":
     with open("settings.json", 'r') as file:
        config = json.load(file)

        map_size = int(str.split(config["MapConfig"]["MapSize"], ',')[0])

        canvas = CanvasGrid(agent_portrayal, map_size, map_size, 500, 500)
        server = ModularServer(FANET, [canvas, text_display], "Random Walk Model", {"config": config})
        server.port = 8521
        server.launch()