#!/usr/bin/env bash

if [ -z $1 ]; then d=$(date -u +%d); else d=$1; fi

d=$(($d+0));

printf -v padded "%02d" $d
day="Day$padded"

if [ -f "AOC2023/Puzzles/$day.cs" ]; then
    echo "Day $padded already exists"
else
    echo "Scaffolding day $d"

    cat "AOC2023/Puzzles/Day00.cs" | sed "s/Day00/$day/g" > "AOC2023/Puzzles/$day.cs"

    touch "AOC2023/Inputs/$padded.txt"

    printf -v testname "%sTest.cs" $day
    cat "Tests/Puzzles/Day00Test.cs" | sed "s/Day00/$day/g" > "Tests/Puzzles/$testname"
fi