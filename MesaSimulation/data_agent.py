from mesa import Model
from moving_agent import MovingAgent
from utils import euclidean_distance

class DataAgent(MovingAgent):

    def __init__(self, unique_id: int, model: Model, x: int, y: int, current_drone, size: int) -> None:
        super().__init__(unique_id, model, x, y)
        
        self.size = size
        self.is_loading = False
        self.current_drone = current_drone
        self.agent_load_to = None
        self.loaded_size = 0

    def start_loading(self, agent_load_to):
        self.is_loading = True
        self.agent_load_to = agent_load_to
        agent_load_to.current_loading_agent = self

    def update_loading(self):
        x1, y1 = self.current_drone.x, self.current_drone.y
        x2, y2 = self.agent_load_to.x, self.agent_load_to.y
        distance = euclidean_distance(x1, y1, x2, y2)
        self.loaded_size = self.loaded_size + self.model.const_loading_speed / (distance**2 + 0.0001)

        new_x = int((x2 - x1) * (self.loaded_size / self.size) + x1)
        new_y = int((y2 - y1) * (self.loaded_size / self.size) + y1)

        if (self.loaded_size >= self.size):
            self.loaded_size = 0
            self.is_loading = False
            self.agent_load_to.current_loading_agent = None
            self.current_drone = self.agent_load_to
            self.agent_load_to = None
            new_x = self.current_drone.x
            new_y = self.current_drone.y

        self.move(new_x, new_y)

