import unittest
import glob

test_files = glob.glob('*_test.py')
module_names = [f.split('.')[0] for f in test_files]
suites = [unittest.defaultTestLoader.loadTestsFromName(name) for name in module_names]
test_suite = unittest.TestSuite(suites)

runner = unittest.TextTestRunner(verbosity=3)
runner.run(test_suite)

# import unittest
# from data_agent_test import TestDataAgent

# if __name__ == '__main__':
#     suite = unittest.TestLoader().loadTestsFromTestCase(TestDataAgent)
#     unittest.TextTestRunner(verbosity=2).run(suite)