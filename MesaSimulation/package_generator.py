from mesa import Agent, Model


class Package:

    def __init__(self, id) -> None:
        self.id = id


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
        self.source_drone.packages.append(Package(self.package_id))
        self.steps_to_generate_package = self.model.data_generate_period

        self.model.logs.append(f"Step {self.model.current_step}: Package {self.package_id} was generated")