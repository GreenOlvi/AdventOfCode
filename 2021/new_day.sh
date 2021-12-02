#!/usr/bin/env bash

if [ -z $1 ]; then exit -1; fi

printf -v padded "%02d" $1
mkdir -p "AOC2021/Day$padded"
cat "AOC2021/Day00/Puzzle.cs" | sed "s/Day00/Day$padded/g" > "AOC2021/Day$padded/Puzzle.cs"

printf -v testname "Day%sTests.cs" $padded
cat "AocTests/Day00Tests.cs" | sed "s/Day00/Day$padded/g" > "AocTests/$testname"

printf -v jqfilter "{ \"profiles\": (.profiles + { \"AOC2021 Day %s\": { \"commandName\": \"Project\", \"commandLineArgs\": \"%d\" } } ) }" $padded $1
cat AOC2021/Properties/launchSettings.json | jq "$jqfilter" > AOC2021/Properties/launchSettings.json
