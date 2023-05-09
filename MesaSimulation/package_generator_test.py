import unittest
from fanet_model import FANET
from package_generator import Package, PackageGenerator
import json
from drone import Drone

class TestPackage(unittest.TestCase):
    def test_init(self):
        package = Package(1, 10)
        self.assertEqual(package.id, 1)
        self.assertEqual(package.creation_step, 10)
        self.assertEqual(package.received_step, 0)
        self.assertFalse(package.is_lost)
        self.assertFalse(package.is_received)


class TestPackageGenerator(unittest.TestCase):
    def setUp(self):
        with open("settings.json", 'r') as file:
            config = json.load(file)
        self.model = FANET(config)
        self.drone = Drone(5, self.model, 0, 0, True, False)
        self.generator = PackageGenerator(1, self.model, self.drone)

    def test_init(self):
        self.assertEqual(self.generator.unique_id, 1)
        self.assertEqual(self.generator.model, self.model)
        self.assertEqual(self.generator.steps_to_generate_package, self.model.data_generate_period)
        self.assertEqual(self.generator.source_drone, self.drone)
        self.assertEqual(self.generator.package_id, 0)

    def test_step_generate_package(self):
        # Generate package
        self.generator.steps_to_generate_package = 0
        self.generator.step()
        self.assertEqual(len(self.model.packages), 1)
        self.assertEqual(len(self.drone.packages), 1)
        package = self.model.packages[0]
        self.assertEqual(package.id, 1)
        self.assertEqual(package.creation_step, self.model.current_step)
        self.assertEqual(package.received_step, 0)
        self.assertFalse(package.is_lost)
        self.assertFalse(package.is_received)

        # Wait some steps and generate another package
        self.generator.steps_to_generate_package = 3
        self.generator.step()
        self.assertEqual(len(self.model.packages), 1)
        self.assertEqual(len(self.drone.packages), 1)
        self.assertEqual(self.generator.package_id, 1)

        # Wait enough steps to generate another package
        self.generator.steps_to_generate_package = 0
        self.generator.step()
        self.assertEqual(len(self.model.packages), 2)
        self.assertEqual(len(self.drone.packages), 2)
        self.assertEqual(self.generator.package_id, 2)

if __name__ == '__main__':
    unittest.main()