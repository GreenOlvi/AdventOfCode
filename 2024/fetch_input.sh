#!/usr/bin/env bash

if [ -z $1 ]; then d=$(date -u +%d); else d=$1; fi

d=$(($d+0));
inputs_dir="AOC2024/Inputs"

echo "Fetching for day $d"

url="https://adventofcode.com/2024/day/$d/input"
echo "Url=$url"

mkdir -p $inputs_dir
printf -v padded "%02d" $d
dest="$inputs_dir/$padded.txt"
echo "Destination=$dest"

ua="github.com/GreenOlvi/AdventOfCode/2024/fetch_input.sh by piotr.szulc86@gmail.com"

curl -v -A "$ua" -b "session=$(cat .session)" -o "$dest" "$url"

