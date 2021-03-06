﻿using System;

using AAEmu.Game.Models.Game.Char;
using AAEmu.Game.Models.Game.Skills.Templates;
using AAEmu.Game.Models.Game.Units;
using AAEmu.Game.Models.Game.World;

namespace AAEmu.Game.Models.Game.Skills.Effects
{
    public class InteractionEffect : EffectTemplate
    {
        public WorldInteractionType WorldInteraction { get; set; }
        public uint DoodadId { get; set; }

        public override bool OnActionTime => false;

        public override void Apply(Unit caster, SkillCaster casterObj, BaseUnit target, SkillCastTarget targetObj,
            CastAction castObj,
            Skill skill, SkillObject skillObject, DateTime time)
        {
            _log.Debug("InteractionEffect, {0}", WorldInteraction);

            var classType = Type.GetType("AAEmu.Game.Models.Game.World.Interactions." + WorldInteraction);
            if (classType == null)
            {
                _log.Error("InteractionEffect, Unknown world interaction: {0}", WorldInteraction);
                return;
            }

            _log.Debug("InteractionEffect, Action: {0}", classType); // TODO help to debug...

            var action = (IWorldInteraction)Activator.CreateInstance(classType);
            action.Execute(caster, casterObj, target, targetObj, skill.Template.Id);

            if (caster is Character character)
            {
                character.Quests.OnInteraction(WorldInteraction);
            }
        }
    }
}
