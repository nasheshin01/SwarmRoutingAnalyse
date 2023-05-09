from mesa import Agent, Model
import random
from data_agent import DataAgent
from package_generator import Package
from rule_logic import check_conditions

class Worker(DataAgent):
    def __init__(self, unique_id: int, model: Model, current_drone, package: Package):
        super().__init__(unique_id, model, current_drone.x, current_drone.y, current_drone, model.package_size)
        self.worker_state = "Sending"
        self.source_drone = self.current_drone
        self.best_path = self.source_drone.best_path
        self.best_path_id = self.source_drone.best_path_id
        self.step_counts_to_go_back = 0
        self.package = package

        self.next_drone = None
        self.is_next_drone_chosen = False
        self.is_next_drone_busy = False
        self.is_next_drone_out_of_radius = False
        self.is_current_drone_source = False
        self.is_current_drone_destination = False

        self.model.logs.append(f"Step {self.model.current_step}: Worker {self.unique_id} was created with package {self.package.id}")

    def step(self):
        if self.is_loading:
            if self.worker_state == "GoingToStart":
                self.step_counts_to_go_back += 1

            self.update_loading()
            return
        
        self.move(self.current_drone.x, self.current_drone.y)

        # Rules logic
        self.next_drone = None
        self.is_next_drone_out_of_radius = False
        self.is_next_drone_busy = False
        self.is_next_drone_chosen = False
        self.is_current_drone_source = self.current_drone.is_source
        self.is_current_drone_destination = self.current_drone.is_destination

        if self.is_current_drone_destination:
            print("k")

        for rule in self.model.rules:
            if rule["AgentType"] != 2:
                continue

            is_conditions_are_true = check_conditions(self, rule)
            if not is_conditions_are_true:
                continue

            if rule["Action"] == "UpdateBestPath":
                self.update_best_path()
            elif rule["Action"] == "LoadToNextDrone":
                self.load_to_next_drone()
            elif rule["Action"] == "ChangeStateToGoingToStart":
                self.change_state_to_going_to_start()
            elif rule["Action"] == "ChangeStateToSend":
                self.change_state_to_send()
            elif rule["Action"] == "ChangeStateToSendEnd":
                self.change_state_to_send_end()
            elif rule["Action"] == "ChooseNextDroneForward":
                self.choose_next_drone_forward()
            elif rule["Action"] == "ChooseNextDroneBack":
                self.choose_next_drone_back()
            elif rule["Action"] == "SetPackageLost":
                self.set_package_lost()
            elif rule["Action"] == "SetPackageReceived":
                self.set_package_received()
            elif rule["Action"] == "DestroyAgent":
                self.destroy_agent()
                return
            elif rule["Action"] == "DoNothing":
                return

    def change_state_to_going_to_start(self):
        self.size = self.model.scout_size
        self.worker_state = "GoingToStart"

    def change_state_to_send(self):
        self.worker_state = "Sending"
    
    def change_state_to_send_end(self):
        self.worker_state = "SendingEnded"

    def update_best_path(self):
        self.current_drone.update_best_path(self.best_path_id, self.step_counts_to_go_back)

    def load_to_next_drone(self):
        self.start_loading(self.next_drone)
   
    def choose_next_drone_forward(self):
        for i in range(len(self.best_path)):
            if self.best_path[i] == self.current_drone:
                self.next_drone = self.best_path[i + 1]
                self.is_next_drone_chosen = True
                self.is_next_drone_busy = self.next_drone.current_loading_agent is not None
                self.is_next_drone_out_of_radius = self.get_distance(self.next_drone) > self.model.max_drone_distance
                break

    def choose_next_drone_back(self):
        for i in range(len(self.best_path)):
            if self.best_path[i] == self.current_drone:
                self.next_drone = self.best_path[i - 1]
                self.is_next_drone_chosen = True
                self.is_next_drone_busy = self.next_drone.current_loading_agent is not None
                self.is_next_drone_out_of_radius = self.get_distance(self.next_drone) > self.model.max_drone_distance
                break

    def set_package_lost(self):
        self.package.is_lost = True

    def set_package_received(self):
        self.package.is_received = True
        self.package.received_step = self.model.current_step

        self.model.logs.append(f"Step {self.model.current_step}: Worker {self.unique_id} has sent package {self.package.id}")

    def destroy_agent(self):
        self.model.schedule.remove(self)
        self.model.grid.remove_agent(self)
        pass