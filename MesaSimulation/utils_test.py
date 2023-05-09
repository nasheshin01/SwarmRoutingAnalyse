import unittest
from utils import euclidean_distance

class TestEuclideanDistance(unittest.TestCase):
    
    def test_same_point(self):
        distance = euclidean_distance(1, 2, 1, 2)
        self.assertEqual(distance, 0)
        
    def test_horizontal_distance(self):
        distance = euclidean_distance(1, 2, 5, 2)
        self.assertEqual(distance, 4)
        
    def test_vertical_distance(self):
        distance = euclidean_distance(1, 2, 1, 6)
        self.assertEqual(distance, 4)
        
    def test_diagonal_distance(self):
        distance = euclidean_distance(1, 2, 4, 6)
        self.assertEqual(distance, 5)
        
    def test_large_distance(self):
        distance = euclidean_distance(1, 2, 100, 200)
        self.assertAlmostEqual(distance, 221.37072977247917)
        
if __name__ == '__main__':
    unittest.main()