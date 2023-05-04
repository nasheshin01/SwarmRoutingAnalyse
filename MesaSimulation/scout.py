from mesa import Agent, Model
import random
from data_agent import DataAgent
from drone import Drone

class Scout(DataAgent):
    def __init__(self, unique_id: int, model: Model, current_drone: Drone, size: int):
        super().__init__(unique_id, model, current_drone.x, current_drone.y, current_drone, size)
        self.path = [current_drone]
        self.scout_state = "Scouting"
        self.source_drone = self.current_drone
        self.energy = self.model.scout_energy_limit
        self.best_path = self.source_drone.best_path
        self.step_counts_to_go_back = 0

    def step(self):
        if self.energy <= 0:
            self.destroy_agent()
            return

        if self.is_loading:
            if self.scout_state == "GoingToStart":
                self.step_counts_to_go_back += 1

            self.update_loading()
            return
        
        self.move(self.current_drone.x, self.current_drone.y)

        if self.scout_state == "Scouting" and self.current_drone.is_destination:
            self.scout_state = "GoingToStart"
        elif self.scout_state == "GoingToStart" and self.current_drone.is_source:
            self.scout_state = "ScoutingEnded"
        
        if self.scout_state == "Scouting":
            self.energy = self.energy - 1
            close_drones = list(filter(lambda d: d.get_distance(self) < self.model.max_drone_distance, self.model.drones))
            close_free_drones = list(filter(lambda d: d.current_loading_agent is None, close_drones))
            close_free_drones = list(filter(lambda d: not (d in self.path), close_free_drones))
           
            if len(close_free_drones) == 0:
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

            self.start_loading(random_drone)
            self.path.append(random_drone)
            
        elif self.scout_state == "GoingToStart":
            next_drone = None
            for i in range(len(self.path)):
                if self.path[i] == self.current_drone:
                    if i == 0:
                        self.destroy_agent()
                        return
                    next_drone = self.path[i - 1]
                    break

            if next_drone is None:
                self.destroy_agent()
                return
            
            if next_drone.current_loading_agent is not None:
                return


            if self.get_distance(next_drone) > self.model.max_drone_distance:
                self.destroy_agent()
                return
            
            self.start_loading(next_drone)
        elif self.scout_state == "ScoutingEnded":
            if self.current_drone.is_source:
                self.current_drone.add_best_path(self.path, self.step_counts_to_go_back)

            self.model.logs.append(f"Step {self.model.current_step}: Scout {self.unique_id} has found new path with step count {self.step_counts_to_go_back}")

            self.destroy_agent()

    def destroy_agent(self):
        self.path = [self.source_drone]
        self.scout_state = "Scouting"
        self.current_drone = self.source_drone 
        self.energy = self.model.scout_energy_limit
        self.best_path = self.current_drone.best_path
        self.step_counts_to_go_back = 0

        pass