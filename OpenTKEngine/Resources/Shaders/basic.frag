#version 130

in vec2 texCoord0;

out vec4 fragColor;

uniform sampler2D texUnit0;

struct Material{
	vec4 diffuse;
	vec4 ambient;
	vec4 specular;
	vec4 emissive;
};

uniform Material material;

void main(){

   vec4 texColor = texture2D(texUnit0, vec2(texCoord0.s, texCoord0.t));

   if(texColor == vec4(0.0,0.0,0.0,0.0))
   {
      texColor = vec4(1.0,1.0,1.0,1.0);
   }

   fragColor = vec4(material.diffuse.rgb*texColor, 1.0);
}