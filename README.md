# VoronoiGrid
Library for grid based Voronoi diagram generation.

This library generates two dimensional region maps, based on Vornoi diagram generation using [Fortunes Sweepline algorithm](https://en.wikipedia.org/wiki/Fortune%27s_algorithm).

I used the C++ implementation of [Ivan Kutskir](http://blog.ivank.net/fortunes-algorithm-and-implementation.html) as reference and source for the breakpoint and circle event calculations. Thanks to Ivan for his code and also for the good explanation of the algorithm on his blog.

Part of the solution is also the quick and dirty WinForms application [`VoronoiViewer`](https://github.com/lachsfilet/VoronoiGrid/tree/master/VoronoiViewer) and the unit tests inside of the project [`VoronoiTests`](https://github.com/lachsfilet/VoronoiGrid/tree/master/VoronoiTests)