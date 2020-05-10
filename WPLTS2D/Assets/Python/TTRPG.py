import random
class TTRPG:
	def __init__(self, master):
		self.skills = {}
		self.m = master
		with open(master.datapath + "SkillsData.txt", "r") as f:
			for x in f.read().split("\n"):
				print(x)
				skill = x.split(" - ")[0]
				desc = x.split(" - ")[1].split("."[0])
				modifiers = []
				for y in x.split(" - ")[1].split(".")[len(x.split(" - ")[1].split("."))-1].split(" "):
					print(y)
					if y.strip() in ["", " "]:
						continue
					modname = y.split("(")[0]
					modpow = int(y.split("(")[1].replace(")", "").replace("M", ""))
					m = "M" in y.split("(")[1]
					modifiers.append([modname, modpow, m])
				self.skills[skill] = [desc, modifiers]
	def D(self, amt):
		return random.randrange(1, amt)
	def SkillCheck(self, character, skill):
		if skill not in self.skills:
			print("Invalid Skill")
			return 0
		d = self.D(20)
		#for each of the modifiers
		for x in self.skills[skill][1]:
			value = character.stats.stats[x[0]] - x[1]
			if value < 0 and x[2] == True:
				value = 0
			print("{} modifier, +{}".format(x[0], str(value)))
			d += value
		d += character.skills[skill]
		print(d)
		return d

