\ source location handling

\ Copyright (C) 1995,1997,2003,2004,2007,2009 Free Software Foundation, Inc.

\ This file is part of Gforth.

\ Gforth is free software; you can redistribute it and/or
\ modify it under the terms of the GNU General Public License
\ as published by the Free Software Foundation, either version 3
\ of the License, or (at your option) any later version.

\ This program is distributed in the hope that it will be useful,
\ but WITHOUT ANY WARRANTY; without even the implied warranty of
\ MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
\ GNU General Public License for more details.

\ You should have received a copy of the GNU General Public License
\ along with this program. If not, see http://www.gnu.org/licenses/.

\ related stuff can be found in kernel.fs

\ this stuff is used by (at least) assert.fs and debugs.fs

: loadfilename#>str ( n -- addr u )
    included-files 2@ rot min 2* cells + 2@ ;

: str>loadfilename# ( addr u -- n )
    included-files 2@ 0 ?do ( addr u included-files )
	i over >r 2* cells + 2@
	2over str= if
	    rdrop 2drop i unloop exit
	endif
	r> loop
    drop 2drop 0 ;

\ we encode line and character in one cell to keep the interface the same
: encode-pos ( nline nchar -- npos )
    $ff min swap 8 lshift + ;

: decode-pos ( npos -- nline nchar )
    dup 8 rshift swap $ff and ;

: current-sourcepos ( -- nfile npos )
    sourcefilename  str>loadfilename# sourceline# >in @ encode-pos ;

: compile-sourcepos ( compile-time: -- ; run-time: -- nfile npos )
    \ compile the current source position as literals: nfile is the
    \ source file index, nline the line number within the file.
    current-sourcepos
    swap postpone literal
    postpone literal ;

: .sourcepos ( nfile npos -- )
    \ print source position
    swap loadfilename#>str type ': emit
    base @ decimal
    swap decode-pos swap 0 .r ': emit 0 .r
    base ! ;


