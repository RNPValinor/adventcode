#!/bin/bash

DAY="day$1"
rm -f puzzles/"$DAY"/puzzle.output
dotnet run -d $1 "$(< puzzles/"$DAY"/puzzle.input)" > puzzles/"$DAY"/puzzle.output
cat puzzles/"$DAY"/puzzle.output