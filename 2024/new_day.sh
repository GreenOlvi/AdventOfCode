#!/usr/bin/env bash

if [ -z $1 ]; then d=$(date -u +%d); else d=$1; fi

d=$(($d+0));
puzzles_dir="AOC2024/Puzzles/"
tests_dir="AOC2024Tests/Puzzles/"

printf -v padded "%02d" $d
export DAY="Day$padded"

if [ -f "$uzzles_dir/$DAY.cs" ]; then
    echo "Day $padded already exists"
else
    echo "Scaffolding day $padded"

    cat "$puzzles_dir/Day00.cs.template" | envsubst > "$puzzles_dir/$DAY.cs"
    cat "$tests_dir/Day00Test.cs.template" | envsubst > "$tests_dir/${DAY}Test.cs"
    touch "AOC2024/Inputs/$padded.txt"
fi
