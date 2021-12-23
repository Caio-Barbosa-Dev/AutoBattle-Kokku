========================================================================
    Turn RPG system

    Kokku Software Engineer Applicant Test
========================================================================

Requisites:
    
    *This test must be done exclusivley on visual studio, do not use any engine.

    This program needs to work with these rules:
    
        - This application is an auto-battle RPG, that has a "grid" with cells/tiles/boxes organized as a matrix.
        - This grid provides the length for the x and y-axis.
        - There is also a Character with a Name, Health, BaseDamage, DamageMultiplier, current cell/box, target, and an Index.
        - Each team should have one move per turn (except when the move places the character in attack range of an opposing team character)
        - The game should work with a "battlefield" of any size, including a non-square matrix.
        - Make sure all the variables in CHARACTER are engaged in a code feature.
        - The game should inform the player when the battle is over and which team has been declared victorious.
        - The battlefield should only be reprinted/redrawn if a player makes a move.
        - Each character should look for a possible target and attack it when this is viable and if not, move closer into attack range.
        - Each candidate must also implement one of the following extra features in the application, to be selected depending on their month of birth.

        Please, document EVERY change you make on the code. Bear in mind we are going to look into how you're able to optimize, organize and refactor.
        
        Tips:
            1 - Make sure the application runs with no errors or warnings.
            2 - Verify that the code is accomplishing every step for all the rules.
            3 - Consider any means to optimize the code, look out for bad performance and memory usage.
            4 - Feel free to modify or refactor anything, as long as you document it.
            5 - The code convention should be consistent, feel free to adjust any part that does not follow this rule.
            6 - The game can't have arbitrary decisions,  example: a player always starts first.


        Each candidate must also implement one of the following extra features in the application, to be selected depending on their month of birth. Please when you deliver
	your test, specify wich of the extra feature you implemented.
        
        Extra Features:
        
            Jan~Feb: Add more characters and divide them into different teams. The game should show clearly what occurs on each turn.;
            Mar~April: A unique feature to each class, EX: paladins with more life, archers can attack in a certain range, etc...;
            May~July: SetAttacks have a chance to push a character away (random chance);
            Aug~Sep: The character can attack/walk in 8 directions;
            Oct~Nov: Add an effect for each class that can somehow paralyze other characters (random chance);
            Dec: Add an Invulnerable (take no damage) and an Empower (more damage) for each class that have a duration of 1 or more turns;
		