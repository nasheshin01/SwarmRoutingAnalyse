from mesa import Agent, Model
from mesa.time import RandomActivation
from mesa.space import MultiGrid, ContinuousSpace
from mesa.visualization.modules import CanvasGrid
from mesa.visualization.ModularVisualization import ModularServer

from drone import Drone
from scout import Scout
from package_generator import PackageGenerator
import random

class FANET(Model):
    """A model of agents moving randomly on a 2D grid."""

    def __init__(self, config):
        # Initialize config data
        self.scout_count = config['ScoutCount']
        self.data_count = config["DataCount"]
        self.data_generate_period = config["DataGeneratePeriod"] 
        self.package_size = config["PackageSize"]
        self.scout_size = config["ScoutSize"]
        self.scout_energy_limit = config["ScoutEnergyLimit"]
        self.max_drone_distance = config["MaxDroneDistance"]
        self.drone_move_period = config["DroneMovePeriod"]
        self.end_step = config["EndSimulationTick"]
        self.const_loading_speed = config["ConstLoadingSpeed"]
        self.init_map = config["MapConfig"]["Map"]
        self.map_size = int(str.split(config["MapConfig"]["MapSize"], ',')[0])
        self.rules = config["Rules"]


        # Initialize simulation
        self.grid = MultiGrid(self.map_size, self.map_size, torus=True)
        self.schedule = RandomActivation(self)
        self.drones = []
        # Create agents
        source_drone = None
        destination_drone = None
        drone_id = 0
        for x in range(self.map_size):
            for y in range(self.map_size):
                reversed_y = self.map_size - y - 1
                if self.init_map[x][y] == 1:
                    agent = Drone(drone_id, self, x, reversed_y, False, False)
                elif self.init_map[x][y] == 2:
                    agent = Drone(drone_id, self, x, reversed_y, True, False)
                    source_drone = agent
                elif self.init_map[x][y] == 3:
                    agent = Drone(drone_id, self, x, reversed_y, False, True)
                else:
                    continue
                
                drone_id += 1
                   
                self.drones.append(agent)

                self.grid.place_agent(agent, (x, reversed_y))
                self.schedule.add(agent)

        for i in range(self.scout_count):
            scout = Scout(self.map_size*self.map_size + i, self, source_drone, self.scout_size)
            self.grid.place_agent(scout, (source_drone.x, source_drone.y))
            self.schedule.add(scout)

        self.schedule.add(PackageGenerator(1313141, self, source_drone))
        self.packages = []
        self.logs = []
        self.current_step = 0

    def step(self):
        self.current_step += 1
        self.schedule.step()