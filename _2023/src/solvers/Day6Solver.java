package solvers;

import java.util.ArrayDeque;
import java.util.Queue;
import java.util.regex.Pattern;

@SuppressWarnings("unused")
public class Day6Solver extends BaseSolver {
    private final Pattern _numberPattern = Pattern.compile("([0-9]+)");

    private final Queue<Integer> _times = new ArrayDeque<>();

    private long _bigTime = 0;

    private int _recordBeatingMult = 1;
    private int _numberOfBigWins = -1;

    public Day6Solver() {
        super(6);
    }

    @Override
    protected void processLine(String line) {
        var matcher = this._numberPattern.matcher(line);

        var bigNumberBuilder = new StringBuilder();

        if (this._times.isEmpty()) {
            // Times
            while (matcher.find()) {
                var group = matcher.group(1);
                this._times.add(Integer.parseInt(group));
                bigNumberBuilder.append(group);
            }

            this._bigTime = Long.parseLong(bigNumberBuilder.toString());
        } else {
            // Records
            while (matcher.find()) {
                var group = matcher.group(1);

                bigNumberBuilder.append(group);

                var record = Integer.parseInt(group);
                var time = this._times.poll();

                if (time == null) {
                    System.err.println("Failed to find a time to go with the record " + record);
                    return;
                }

                this._recordBeatingMult *= this.FindWinningWays(time, record);
            }

            var bigRecord = Long.parseLong(bigNumberBuilder.toString());

            this._numberOfBigWins = this.FindWinningWays(this._bigTime, bigRecord);
        }
    }

    private int FindWinningWays(long totalTime, long record) {
        // Find all solutions of t for (totalTime - t) * t > record
        // t^2 - t*totalTime + record = 0
        // t = totalTime +- Sqrt(-totalTime ^ 2 - 4 * 1 * record) / 2

        var sqrt = Math.sqrt(Math.pow(-totalTime, 2) - 4 * record);

        var isPerfectSquare = (Math.floor(sqrt) - sqrt) == 0;

        var minTime = (int) Math.ceil((totalTime - sqrt) / 2);
        var maxTime = (int) Math.floor((totalTime + sqrt) / 2);

        if (isPerfectSquare) {
            // In this case the start and end times only match
            // the record, rather than beating it.
            minTime++;
            maxTime--;
        }

        return maxTime - minTime + 1;
    }

    @Override
    protected String solvePart1() {
        return String.valueOf(this._recordBeatingMult);
    }

    @Override
    protected String solvePart2() {
        return String.valueOf(this._numberOfBigWins);
    }
}
