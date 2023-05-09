from mesa import Agent, Model


class Package:

    def __init__(self, id, step) -> None:
        self.id = id
        self.creation_step = step
        self.received_step = 0
        self.is_lost = False
        self.is_received = False


class PackageGenerator(Agent):

    def __init__(self, unique_id: int, model: Model, source_drone) -> None:
        super().__init__(unique_id, model)

        self.steps_to_generate_package = self.model.data_generate_period
        self.source_drone = source_drone
        self.package_id = 0

    def step(self):
        if self.steps_to_generate_package != 0:
            self.steps_to_generate_package -= 1
            return
        
        self.package_id += 1
        package = Package(self.package_id, self.model.current_step)
        self.model.packages.append(package)
        self.source_drone.packages.append(package)
        self.steps_to_generate_package = self.model.data_generate_period

        self.model.logs.append(f"Step {self.model.current_step}: Package {self.package_id} was generated")