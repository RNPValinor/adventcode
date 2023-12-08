package solvers;

import utils.Maths;
import utils.Tuple;

import java.util.*;
import java.util.regex.Pattern;

@SuppressWarnings("unused")
public class Day8Solver extends BaseSolver {
    private final HashMap<String, String[]> _map = new HashMap<>();
    private final List<Integer> _instructions = new ArrayList<>();

    public Day8Solver() {
        super(8);
    }

    @Override
    protected void ProcessLine(String line) {
        if (this._instructions.isEmpty()) {
            for (var i = 0; i < line.length(); i++) {
                switch (line.charAt(i)) {
                    case 'L':
                        this._instructions.add(0);
                        break;
                    case 'R':
                        this._instructions.add(1);
                        break;
                }
            }
        } else if (!line.isEmpty()) {
            var node = line.substring(0, 3);
            var left = line.substring(7, 10);
            var right = line.substring(12, 15);

            this._map.put(node, new String[]{left, right});
        }
    }

    @Override
    protected String SolvePart1() {
        var instructionIndex = 0;
        var curPos = "AAA";
        var numSteps = 0;

        while (!curPos.equals("ZZZ")) {
            var instruction = this._instructions.get(instructionIndex);

            curPos = this._map.get(curPos)[instruction];
            numSteps++;

            instructionIndex = (instructionIndex + 1) % this._instructions.size();
        }

        return String.valueOf(numSteps);
    }

    @Override
    protected String SolvePart2() {
        var numSteps = 1L;

        var nodes = this._map
                .keySet()
                .stream()
                .filter(n -> n.charAt(2) == 'A')
                .toArray(String[]::new);
        var found = 0;
        var foundMask = 0;
        var stepIdx = 0;

        while (found < nodes.length) {
            var instruction = this._instructions.get(stepIdx++ % this._instructions.size());

            for (int i = 0; i < nodes.length; i++) {
                nodes[i] = this._map.get(nodes[i])[instruction];
            }

            for (int i = 0; i < nodes.length; i++) {
                if ((foundMask & (1 << i)) == 0 && nodes[i].charAt(2) == 'Z') {
                    foundMask |= 1 << i;
                    found++;

                    numSteps = Maths.lcm(numSteps, stepIdx);
                }
            }
        }

        if (numSteps <= 14541741720L) {
            System.err.println("Part 2 answer too low");
        }

        if (numSteps >= 3080679468441011440L) {
            System.err.println("Part 2 answer too high");
        }

        return String.valueOf(numSteps);
    }

    private enum Instruction {
        L,
        R
    }
}
