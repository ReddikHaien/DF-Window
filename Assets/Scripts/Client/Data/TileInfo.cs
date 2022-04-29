using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfFortress.TileInfo{
    public class UniqueManager<T>{
        private HashSet<T> collection;
        public UniqueManager(){
            this.collection = new HashSet<T>();
        }

        public T GetUnique(T value){
            if(collection.TryGetValue(value, out var existing)){
                return existing;
            }
            else{
                collection.Add(value);
                return value;
            }
        }

        public int Size => collection.Count;
    }

    public struct MatPair{
        public readonly int Type;
        public readonly int Index;

        public MatPair(int type, int index){
            this.Type = type;
            this.Index = index;
        }

        public override bool Equals(object obj)
        {
            if (obj is MatPair other){
                return other.Index == this.Index && other.Type == this.Type;
            }
            else{
                return false;
            }
        }

        public override int GetHashCode()
        {
            var code = new HashCode();
            code.Add(Type);
            code.Add(Index);
            return code.ToHashCode();
        }


        public override string ToString() => $"MatPair[type: {Type}, index: {Index}]";
    }

    public struct DfColor{
        public readonly byte R;
        public readonly byte G;
        public readonly byte B;

        public DfColor(byte r, byte g, byte b){
            R = r;
            G = g;
            B = b;
        }

        public override bool Equals(object obj)
        {
            if (obj is DfColor other){
                return other.R == this.R && other.G == this.G && other.B == this.B;
            }
            else{
                return false;
            }
        }

        public override int GetHashCode()
        {
            var code = new HashCode();
            code.Add(R);
            code.Add(G);
            code.Add(B);
            return code.ToHashCode();
        }        
    }

    public class Tile{
        public int TileType;

        public MatPair Material;
        public MatPair BaseMaterial;
        public MatPair LayerMaterial;
        public MatPair VeinMaterial;
        public MatPair ConstructionMaterial;
        public int WaterLevel;
        public int MagmaLevel;
        public int RampType;
        public bool Hidden;
        public byte TrunkPercent;

        public Tile(){

        }

        public Tile(
            int tileType,
            MatPair material,
            MatPair baseMaterial,
            MatPair layerMaterial,
            MatPair veinMaterial,
            MatPair constructionMaterial,
            int waterLevel,
            int magmaLevel,
            int rampType,
            bool hidden,
            byte trunkPercent
        ){
            TileType = tileType;
            Material = material;
            BaseMaterial = baseMaterial;
            LayerMaterial = layerMaterial;
            VeinMaterial = veinMaterial;
            ConstructionMaterial = constructionMaterial;
            WaterLevel = waterLevel;
            MagmaLevel = magmaLevel;
            RampType = rampType;
            Hidden = hidden;
            TrunkPercent = trunkPercent;
        }
    }

    public class MaterialDefinition{
        public MatPair MatPair { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public DfColor Color { get; set; }
        public ItemdefInstrument.InstrumentDef InstrumentDef { get; set; }
        public int UpStep { get; set; }
        public int DownStep { get; set; }
        public RemoteFortressReader.ArmorLayer ArmorLayer { get; set; }
    }
}