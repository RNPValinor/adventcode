package solvers;

import java.io.BufferedReader;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.IOException;

public abstract class BaseSolver {
    protected int _day;

    protected BaseSolver(int day)
    {
        this._day = day;
    }

    public void Solve()
    {
        try (BufferedReader br = new BufferedReader(new FileReader("inputs/day" + this._day + ".txt")))
        {
            String line = br.readLine();

            while (line != null)
            {
                this.ProcessLine(line);
                line = br.readLine();
            }
        }
        catch (FileNotFoundException e)
        {
            System.out.println("Could not find input file for day " + this._day);
            System.out.println(e.getMessage());
        }
        catch (IOException e)
        {
            System.out.println("Failed to read input file for day " + this._day);
            System.out.println(e.getMessage());
        }

        System.out.println();
        System.out.println("Solution for day " + this._day);
        System.out.println();
        System.out.println("Part 1:");
        System.out.println(this.SolvePart1());
        System.out.println();
        System.out.println("Part 2:");
        System.out.println(this.SolvePart2());
    }

    protected abstract void ProcessLine(String line);

    protected abstract String SolvePart1();

    protected abstract String SolvePart2();
}
