from moving_agent import MovingAgent
from enum import Enum
import random
from worker import Worker
from rule_logic import check_conditions

class Drone(MovingAgent):
    def __init__(self, unique_id, model, x: int, y: int, is_source: bool, is_destination: bool):
        super().__init__(unique_id, model, x, y)
        self.energy = 10
        self.current_loading_agent = None
        self.is_source = is_source
        self.is_destination = is_destination
        self.steps_to_wait = self.model.drone_move_period
        self.best_path = []
        self.best_path_step_count = 100000000
        self.best_path_id = 0
        self.packages = []
        self.worker_id = 312314
        self.hibernation_status = "NoHibernate"

    def step(self):
        if len(self.best_path) != 0 and len(self.packages) != 0:
            self.create_worker()

        if self.steps_to_wait != 0:
            self.steps_to_wait = self.steps_to_wait - 1
            return
        
        for rule in self.model.rules:
            if rule["AgentType"] != 0:
                continue

            is_conditions_are_true = check_conditions(self, rule)
            if not is_conditions_are_true:
                continue

            if rule["Action"] == "EnableHibernation":
                self.enable_hibernation()
                self.model.logs.append(f"Step {self.model.current_step}: Drone {self.unique_id} enabled hibernation")
            elif rule["Action"] == "DisableHibernation":
                self.disable_hibernation()
                self.model.logs.append(f"Step {self.model.current_step}: Drone {self.unique_id} disabled hibernation")
            elif rule["Action"] == "RandomMove":
                self.random_move((self.model.grid.width, self.model.grid.height))

        self.energy += int(self.hibernation_status == "Hibernate")
        self.energy -= int(self.hibernation_status == "NoHibernate")
        self.steps_to_wait = self.model.drone_move_period

    def enable_hibernation(self):
        self.hibernation_status = "Hibernate"

    def disable_hibernation(self):
        self.hibernation_status = "NoHibernate"

    def random_move(self, boundaries):
        dx = self.random.randrange(-1, 2)
        dy = self.random.randrange(-1, 2)

        if self.x + dx < 0 or self.x + dx >= boundaries[0]:
            dx = 0
        if self.y + dy < 0 or self.y + dy  >= boundaries[1]:
            dy = 0

        self.move(self.x + dx, self.y + dy)


    def add_best_path(self, path, step_count):
        if step_count < self.best_path_step_count:
            self.best_path = path
            self.best_path_step_count = step_count
            self.best_path_id += 1

    def update_best_path(self, step_count, path_id):
        if path_id == self.best_path_id:
            self.best_path_step_count = step_count

    def create_worker(self):
        worker = Worker(self.worker_id, self.model, self, self.packages.pop())
        self.model.grid.place_agent(worker, (self.x, self.y))
        self.model.schedule.add(worker)
        self.worker_id += 1
