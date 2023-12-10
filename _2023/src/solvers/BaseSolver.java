package solvers;

import utils.NanoClock;

import java.io.BufferedReader;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.IOException;

public abstract class BaseSolver {
    protected int _day;

    protected BaseSolver(int day) {
        this._day = day;
    }

    public void Solve() {
        var clock = NanoClock.systemDefaultZone();
        long start = 0;

        try (BufferedReader br = new BufferedReader(new FileReader("inputs/day" + this._day + ".txt"))) {
            start = clock.nanos();

            String line = br.readLine();

            while (line != null) {
                this.processLine(line);
                line = br.readLine();
            }
        } catch (FileNotFoundException e) {
            System.out.println("Could not find input file for day " + this._day);
            System.out.println(e.getMessage());
        } catch (IOException e) {
            System.out.println("Failed to read input file for day " + this._day);
            System.out.println(e.getMessage());
        }

        var lineProcessTimeDone = clock.nanos();

        var part1Solution = this.solvePart1();
        var part1TimeDone = clock.nanos();

        var part2Solution = this.solvePart2();
        var part2TimeDone = clock.nanos();

        System.out.println();
        System.out.println("Solution for day " + this._day);
        System.out.println();
        System.out.println("Part 1:");
        System.out.println(part1Solution);
        System.out.println();
        System.out.println("Part 2:");
        System.out.println(part2Solution);

        System.out.println();
        System.out.println();
        System.out.println("Lines processed in " + (lineProcessTimeDone - start) + "ns");
        System.out.println("Part 1 solved in " + (part1TimeDone - lineProcessTimeDone) + "ns");
        System.out.println("Part 2 solved in " + (part2TimeDone - part1TimeDone) + "ns");
        System.out.println("Total solve time: " + (part2TimeDone - start) + "ns");
    }

    protected abstract void processLine(String line);

    protected abstract String solvePart1();

    protected abstract String solvePart2();
}
