package solvers;

import java.awt.*;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

@SuppressWarnings("unused")
public class Day11Solver extends BaseSolver {
    private final List<Point> _expandedGalaxies = new ArrayList<>();
    private final List<Point> _superExpandedGalaxies = new ArrayList<>();

    private int _numEmptyRows = 0;
    private Boolean[] _emptyColumns;

    private int _y = 0;

    public Day11Solver() {
        super(11);
    }

    @Override
    protected void processLine(String line) {
        var noGalaxies = true;

        if (this._emptyColumns == null) {
            this._emptyColumns = new Boolean[line.length()];
            Arrays.fill(this._emptyColumns, true);
        }

        for (var x = 0; x < line.length(); x++) {
            if (line.charAt(x) == '#') {
                noGalaxies = false;
                this._emptyColumns[x] = false;
                this._expandedGalaxies.add(new Point(x, this._y + this._numEmptyRows));
                this._superExpandedGalaxies.add(new Point(x, this._y + (this._numEmptyRows * 999999)));
            }
        }

        if (noGalaxies) {
            this._numEmptyRows++;
        }

        this._y++;
    }

    private void doXExpansion() {
        for (var i = 0; i < this._expandedGalaxies.size(); i++) {
            var g1 = this._expandedGalaxies.get(i);
            var g2 = this._superExpandedGalaxies.get(i);

            var numToExpandBy = (int) Arrays.stream(this._emptyColumns)
                    .limit(g1.x)
                    .filter(c -> c)
                    .count();

            g1.translate(numToExpandBy, 0);
            g2.translate(numToExpandBy * 999999, 0);
        }
    }

    @Override
    protected String solvePart1() {
        this.doXExpansion();

        var totalDistances = this.getTotalGalaxyDistances(this._expandedGalaxies);

        return String.valueOf(totalDistances);
    }

    private long getTotalGalaxyDistances(List<Point> galaxies) {
        var totalDistances = 0L;

        for (var i = 0; i < galaxies.size(); i++) {
            var g1 = galaxies.get(i);

            for (var j = i + 1; j < galaxies.size(); j++) {
                var g2 = galaxies.get(j);

                totalDistances += this.getDistanceBetween(g1, g2);
            }
        }

        return totalDistances;
    }

    private long getDistanceBetween(Point p1, Point p2) {
        var minX = Math.min(p1.x, p2.x);
        var maxX = Math.max(p1.x, p2.x);

        var minY = Math.min(p1.y, p2.y);
        var maxY = Math.max(p1.y, p2.y);

        return (maxX - minX) + (maxY - minY);
    }

    @Override
    protected String solvePart2() {
        var totalDistances = this.getTotalGalaxyDistances(this._superExpandedGalaxies);

        return String.valueOf(totalDistances);
    }
}
