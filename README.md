# Rasterization
Rasterization application which provides a toolset for drawing multiple shapes with using different algorithms.

## Drawing Algorithms
### Line Drawing
1. DDA Algorithm for drawing lines.
2. DDA Algorithm with Copy Pixels technic for drawing thick lines.
3. Xiaolin Wu's Line Algorithm for applying antialiasing to lines.

### Circle Drawing
1. Midpoint Circle Algorithm for drawing lines.
2. Xiaolin Wu's Circle Algorithm for applying antialiasing to circles.

### Polygon Drawing
1. Drawing polygons with using DDA Line Algorithm.
2. Drawing antialiased algorithms with using Xiaolin Wu's Line Algorithm.

### Rectangle Drawing
1. Drawing rectangles with using DDA Line Algorithm.

## Clipping Algorithm
Liang-Barsky Algorithm is used to clipping polygons edges to a rectangle.

## Filling Algorithms
1. Scan-Line Algorithm with Active Edge Table is used to filling polygons with solid colors.
2. Modified Scan-Line Algorithm with Active Edge Table is used to filling polygons with images.
