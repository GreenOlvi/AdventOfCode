#!/usr/bin/env perl

use strict;
use warnings;
use v5.10;

foreach my $l (<>) {
	my $f = 0;
	foreach my $s (split '', $l) {
		if ($s eq '(') {
			$f++;
		} elsif ($s eq ')') {
			$f--;
		}
	}
	say $f;
}
