#!/bin/bash

DAY="day$1"
rm -f puzzles/"$DAY"/puzzle.output
dotnet run -d $1 "$(< puzzles/"$DAY"/dammit-joe.input)" > puzzles/"$DAY"/dammit-joe.output
cat puzzles/"$DAY"/dammit-joe.output