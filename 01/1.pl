#!/usr/bin/env perl

use strict;
use warnings;
use v5.10;

foreach my $l (<>) {
	my $basement = 0;
	my $pos = 1;
	my $f = 0;
	foreach my $s (split '', $l) {
		if ($s eq '(') {
			$f++;
		} elsif ($s eq ')') {
			$f--;
		}
		if (!$basement && $f < 0) {
			say "Basement at " . $pos;
			$basement = 1;
		}

		$pos++
	}
	say "Ended up at " . $f;
}
