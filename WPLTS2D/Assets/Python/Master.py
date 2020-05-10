import datetime
import sys
sys.path.append(sys.path[1]+"/Python")
from Inventory import Item, Inventory
from TTRPG import TTRPG
class Master:
	def __init__(self, datapath):
		self.datapath = datapath
		self.characters = {}
		self.GM = None
		self.TTRPG = TTRPG(self)
		print("Lore Master Initiated.")
		with open(self.datapath + "logs.txt", "w+") as f:
			f.write("WPLTS Python Log File\n\n")
		self.log("Datapath: " + str(datapath))
		self.log("Path: " + str(sys.path))

	def AddCharacter(self, name):
		c = Character(name, self)
		if name not in self.characters:
			self.characters[name] = c
		else:
			self.log("Character {} has already been created. ".format(name))
		return c
	def GetTest(self):
		return self.GM.Test
		with open(self.datapath+"testfile.txt", "r") as f:
			return f.read()
		return "error?"
	def log(self, txt):
		with open(self.datapath+"logs.txt", "a") as f:
			f.write("\n"+str(datetime.datetime.now()) + "	" + txt)
		print(txt)


class Character:
	def __init__(self, name, master):
		self.name = name
		self.m = master
		self.inventory = Inventory(name, master)
		self.stats = Stats(self, master)
		self.skills = {}
		for x in master.TTRPG.skills:
			self.skills[x] = 0
class Stats:
	def __init__(self, character, master):
		self.m = master
		self.c = character
		self.stats = {}
		for x in "Beef Dexterity Brainpower Glamour Agility".split(" "):
			self.stats[x] = 5
		self.maxhealth = 100
		self.health = 100
		

