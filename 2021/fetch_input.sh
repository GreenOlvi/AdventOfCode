#!/usr/bin/env bash

if [ -z $1 ]; then exit -1; fi

printf -v padded "%02d" $1
curl -v -b "session=$(cat .session)" -o "AOC2021/input/$padded.txt" https://adventofcode.com/2021/day/$1/input
