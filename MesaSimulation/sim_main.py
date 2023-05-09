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

        step_count = config["EndSimulationTick"]

        model = FANET(config)

        for i in range(int(step_count)):
            model.step()


        received_count = 0
        lost_count = 0
        not_sent_packages = 0
        step_sum = 0
        for package in model.packages:
            received_count += int(package.is_received)
            lost_count += int(package.is_lost)
            if package.is_received:
                step_sum += package.received_step - package.creation_step

            if not package.is_received and not package.is_lost:
                not_sent_packages += 1

        

        with open("out.txt", 'w') as out:
            out.write(str(received_count) + '\n')
            out.write(str(lost_count) + '\n')
            out.write(str(not_sent_packages) + '\n')
            out.write(str(step_sum / len(model.packages)) + '\n')
        

        