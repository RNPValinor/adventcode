package utils;

import org.apache.commons.lang3.StringUtils;

import java.util.*;

public class NonogramSolver {
    public static int getNumCombinations(String knownFilled, int[] blockSizes) {
        var blocks = new ArrayList<String>(blockSizes.length);

        for (var size : blockSizes) {
            blocks.add(StringUtils.repeat("#", size));
        }

        var combinations = new HashSet<String>();
        var numSpaces = knownFilled.length() - blockSizes.length - 1 - Arrays.stream(blockSizes).sum();

        generateCombinations(knownFilled, blocks, combinations, "", 0, numSpaces);

        return combinations.size();
    }

    public static void generateCombinations(String knownFilled, List<String> blocks, HashSet<String> combinations, String permutation, int nextBlockIdx, int numSpaces) {
        if (permutation.length() >= knownFilled.length()) {
            if (validateSolution(knownFilled, permutation)) {
                combinations.add(permutation);
            }
        } else {
            StringBuilder nextPermutation = new StringBuilder(permutation);
            var nextBlock = blocks.get(nextBlockIdx);

            while (numSpaces >= 0) {
                generateCombinations(knownFilled, blocks, combinations, nextPermutation + nextBlock + '.', nextBlockIdx + 1, numSpaces);
                nextPermutation.append('.');
                numSpaces--;
            }
        }
    }

    private static boolean validateSolution(String knownFilled, String solution) {
        for (var i = 0; i < knownFilled.length(); i++) {
            var knownChar = knownFilled.charAt(i);
            var solutionChar = solution.charAt(i);

            if (knownChar != '?' && knownChar != solutionChar) {
                return false;
            }
        }

        return true;
    }
}
