from mesa import Agent, Model
import random
from data_agent import DataAgent
from package_generator import Package

class Worker(DataAgent):
    def __init__(self, unique_id: int, model: Model, current_drone, package: Package):
        super().__init__(unique_id, model, current_drone.x, current_drone.y, current_drone, model.package_size)
        self.worker_state = "Sending"
        self.source_drone = self.current_drone
        self.best_path = self.source_drone.best_path
        self.best_path_id = self.source_drone.best_path_id
        self.step_counts_to_go_back = 0
        self.is_package_sent = False
        self.package = package

        self.model.logs.append(f"Step {self.model.current_step}: Worker {self.unique_id} was created with package {self.package.id}")

    def step(self):
        if self.is_loading:
            if self.worker_state == "GoingToStart":
                self.step_counts_to_go_back += 1

            self.update_loading()
            return
        
        self.move(self.current_drone.x, self.current_drone.y)

        if self.worker_state == "Sending" and self.current_drone.is_destination:
            self.is_package_sent = True
            self.model.logs.append(f"Step {self.model.current_step}: Worker {self.unique_id} has sent package {self.package.id}")
            self.worker_state = "GoingToStart"
        elif self.worker_state == "GoingToStart" and self.current_drone.is_source:
            self.worker_state = "SendingEnded"
        
        if self.worker_state == "Sending":
            next_drone = None
            for i in range(len(self.best_path)):
                if self.best_path[i] == self.current_drone:
                    next_drone = self.best_path[i + 1]
                    break

            if next_drone is None:
                self.destroy_agent()
                return
                
            if self.get_distance(next_drone) > self.model.max_drone_distance:
                self.worker_state = "GoingToStart"
                return
                
            if next_drone.current_loading_agent is not None:
                return

            self.start_loading(next_drone)
        elif self.worker_state == "GoingToStart":
            next_drone = None
            for i in range(len(self.best_path)):
                if self.best_path[i] == self.current_drone:
                    if i == 0:
                        self.destroy_agent()
                        return
                    next_drone = self.best_path[i - 1]
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
        elif self.worker_state == "SendingEnded":
            if self.current_drone.is_source:
                self.current_drone.update_best_path(self.best_path_id, self.step_counts_to_go_back)

            self.destroy_agent()

    def destroy_agent(self):
        self.model.schedule.remove(self)
        self.model.grid.remove_agent(self)
        pass