from mesa import Agent, Model
import random
from data_agent import DataAgent
from drone import Drone
from rule_logic import check_conditions

class Scout(DataAgent):
    def __init__(self, unique_id: int, model: Model, current_drone: Drone, size: int):
        super().__init__(unique_id, model, current_drone.x, current_drone.y, current_drone, size)
        self.path = [current_drone]
        self.scout_state = "Scouting"
        self.source_drone = self.current_drone
        self.energy = self.model.scout_energy_limit
        self.best_path = self.source_drone.best_path
        self.step_counts_to_go_back = 0

        self.next_drone = None
        self.is_next_drone_out_of_radius = False
        self.is_next_drone_in_loading_state = False
        self.is_next_drone_choosed = False
        self.is_current_drone_source = False
        self.is_current_drone_destination = False

    def step(self):
        # Default things
        if self.is_loading:
            if self.scout_state == "GoingToStart":
                self.step_counts_to_go_back += 1

            self.update_loading()
            return
        
        self.move(self.current_drone.x, self.current_drone.y)

        # Rules logic
        self.next_drone = None
        self.is_next_drone_out_of_radius = False
        self.is_next_drone_in_loading_state = False
        self.is_next_drone_choosed = False
        self.is_current_drone_source = self.current_drone.is_source
        self.is_current_drone_destination = self.current_drone.is_destination

        self.energy -= int(self.scout_state == "Scouting")

        for rule in self.model.rules:
            if rule["AgentType"] != 1:
                continue

            is_conditions_are_true = check_conditions(self, rule)
            if not is_conditions_are_true:
                continue

            if rule["Action"] == "AddBestPath":
                self.add_best_path()
            elif rule["Action"] == "LoadToNextDrone":
                self.load_to_next_drone()
            elif rule["Action"] == "ChangeStateToGoingToStart":
                self.change_state_to_going_to_start()
            elif rule["Action"] == "ChangeStateToScout":
                self.change_state_to_scout()
            elif rule["Action"] == "ChangeStateToScoutEnd":
                self.change_state_to_scout_end()
            elif rule["Action"] == "ChooseNextDroneToGoBack":
                self.choose_next_drone_to_go_back()
            elif rule["Action"] == "ChooseNextRandomDrone":
                self.choose_next_random_drone()
            elif rule["Action"] == "DestroyAgent":
                self.destroy_agent()
                return
            

    def add_best_path(self):
         self.current_drone.add_best_path(self.path, self.step_counts_to_go_back)

    def load_to_next_drone(self):
        self.start_loading(self.next_drone)

        if self.scout_state == "Scouting":
            self.path.append(self.next_drone)

    def change_state_to_going_to_start(self):
        self.scout_state = "GoingToStart"

    def change_state_to_scout(self):
        self.scout_state = "Scouting"
    
    def change_state_to_scout_end(self):
        self.scout_state = "ScoutingEnded"

    def choose_next_drone_to_go_back(self):
        next_drone = None
        for i in range(len(self.path)):
            if self.path[i] == self.current_drone:
                next_drone = self.path[i - 1]
                break

        self.next_drone = next_drone
        self.is_next_drone_in_loading_state = next_drone.current_loading_agent is not None
        self.is_next_drone_out_of_radius = self.get_distance(next_drone) > self.model.max_drone_distance
        self.is_next_drone_choosed = True


    def choose_next_random_drone(self):
        close_drones = list(filter(lambda d: d.get_distance(self) < self.model.max_drone_distance, self.model.drones))
        close_free_drones = list(filter(lambda d: d.current_loading_agent is None, close_drones))
        close_free_drones = list(filter(lambda d: not (d in self.path), close_free_drones))
        
        if len(close_free_drones) == 0:
            self.is_next_drone_choosed = False
            return

        if len(self.best_path) != 0:
            best_drones = list(filter(lambda d: (d in self.best_path), close_free_drones))
            not_best_drones = list(filter(lambda d: not (d in self.best_path), close_free_drones))
            if len(best_drones) == 0:
                random_drone = random.choice(not_best_drones)
            elif len(not_best_drones) == 0:
                random_drone = random.choice(best_drones)
            elif (random.randrange(0, 9) > 0):
                random_drone = random.choice(best_drones)
            else:
                random_drone = random.choice(not_best_drones)
        else:
            random_drone = random.choice(close_free_drones)

        self.next_drone = random_drone
        self.is_next_drone_in_loading_state = random_drone.current_loading_agent is not None
        self.is_next_drone_out_of_radius = self.get_distance(random_drone) > self.model.max_drone_distance
        self.is_next_drone_choosed = True


    def destroy_agent(self):
        self.path = [self.source_drone]
        self.scout_state = "Scouting"
        self.current_drone = self.source_drone 
        self.energy = self.model.scout_energy_limit
        self.best_path = self.current_drone.best_path
        self.step_counts_to_go_back = 0