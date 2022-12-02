#!/usr/bin/env bash

if [ -z $1 ]; then d=$(date -u +%d); else d=$1; fi

d=$(($d+0));

echo "Fetching for day $d"

url="https://adventofcode.com/2022/day/$d/input"
echo "Url=$url"

printf -v padded "%02d" $d
dest="AOC2022/Inputs/$padded.txt"
echo "Destination=$dest"

ua="github.com/GreenOlvi/AdventOfCode/2022/fetch_input.sh by piotr.szulc86@gmail.com"

curl -v -A "$ua" -b "session=$(cat .session)" -o "$dest" "$url"

