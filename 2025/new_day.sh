#!/usr/bin/env bash

if [ -z $1 ]; then d=$(date -u +%d); else d=$1; fi

d=$(($d+0));
puzzles_dir="AOC2025/Puzzles"
tests_dir="AOC2025Tests/Puzzles"
inputs_dir="Inputs"

printf -v padded "%02d" $d
export DAY="Day$padded"

if [ -f "$puzzles_dir/$DAY.cs" ]; then
    echo "Day $padded already exists"
else
    echo "Scaffolding day $padded"

    cat "$puzzles_dir/Day00.cs.template" | envsubst > "$puzzles_dir/$DAY.cs"
    cat "$tests_dir/Day00Test.cs.template" | envsubst > "$tests_dir/${DAY}Test.cs"
    touch "$inputs_dir/$padded.txt"
fi
