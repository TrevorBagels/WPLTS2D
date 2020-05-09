class Inventory:
	def __init__(self, name, master):
		self.name = name
		self.items = []
		self.equipment = Equipment(self)
		self.capacity = 10
		self.m = master
	def LoadItemFromXML(self):
		return Item("test", "test", 1, "generic")
	def Item(self, name, desc, ID, t):
		return Item(name, desc, ID, t)
	def AddItem(self, itm):
		returnvalue = {"status": "success", "reason": "none"}
		if len(self.items >= 10):
			self.m.log("Inventory capacity reached. Item {} will not be added.".format(itm.name))
		else:
			self.m.log("Adding item {} to inventory {}".format(itm.name, self.name))
			self.items.append(itm)

class Equipment:
	def __init__(self, inv):
		self.inv = inv
		self.slots = [None, None, None, None]
		self.head = None
		self.chest = None
		self.legs = None
		self.gloves = None
		self.shield = None
		self.trinkets = [None, None, None]  


class Item:
	def __init__(self, name, desc, ID, t, value=50, tags=[], subtype=""):
		self.name = name
		self.desc = desc
		self.id = ID
		self.type = t
		self.value = value
		self.tags = tags
		self.subtype = subtype
		self.properties = {}