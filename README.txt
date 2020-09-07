"Hattrick ASP.NET Example" provides basic OAuth from Hattrick.org and examples of data retrieval. 

Copyright (C) 2020 Tobias Jønch

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.

-------------------------------------------------------------------------

This is the start of my own Hattrick CHPP product 'HT-Stats'.
It is not by any means complete or mature and the code will reflect that in many places.
Some of it is even untested.

The project provides very basic authentication to Hattrick's CHPP backend, and shows how the data
might be fetched and stored in a MySQL database. Performance has not been a priority at this 
stage, so there's almost certainly issues in that area. The various methods to fetch the data use 
somewhat different approaches, with the UpdateMatchInfo method being perhaps the most 'mature'. 

For security reasons I obviously can't leave the data tables here, but I have provided a 
diagram of the current database schema, which I consider sound but incomplete. The specific model
used can be reviewed in Models/ht_stats_dk_dbContext.cs.

The project is based on the ASP.NET Core webapp template provided by Visual Studio and uses 
Entity Framework 6.

There is absolutely no work done on the webpages themselves, this has been all about data retrieval
so far. It has been successfully deployed to an IIS server, though. 
The fun starts in Startup.cs, not surprisingly, and continues in ChppAccess.cs, DbUpdate.cs and 
XmlParser.cs. 

In the hopes someone might find it useful,
Tobias Jønch

7 September 2020, Copenhagen